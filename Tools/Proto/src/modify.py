import os
import re
from turtledemo.penrose import start

import config

# cs 文件名称
cs_file_pattern = r'^([A-Za-z_]+)(\d+)\.cs$'
# cs 类名称
cs_class_pattern = r'^\s*public sealed partial class (\w+)\s+:\s+pb::IMessage<'
# cs 命名空间 namespace ET {
cs_namespace_pattern = r'^namespace ET {$'

# proto 消息 不带注解
proto_msg_pattern = r'\s*message\s+(\w+)\s*\{?\s*$'
# proto 返回类型  // ResponseType ObjectQueryResponse
proto_res_type_pattern = r'//\s+res\s+(\w+)'
# proto 消息类型   // IRequest
proto_msg_type_pattern = r'\s*message\s+(\w+)\s+//\s*(\w+)'

opcode_class = r"""
public static class {0}
{{
{1}
}}
"""

opcode_item = ' public const ushort {0} = {1};\n'


def modify(language):
    print(f"start modify {language} =========================")
    output_path = config.output_path[language]
    if output_path is None:
        print(f"{language} not support")
        return

    if not os.path.exists(output_path):
        print(f"{language} modify failed, please generate code first!")
        return

    for root, _, files in os.walk(output_path):
        for file in files:
            if not file.endswith(".cs"):
                continue

            match_result = re.match(cs_file_pattern, file)
            message_name = ""
            start_id = 0
            if match_result:
                message_name = match_result.group(1)
                start_id = int(match_result.group(2))
            else:
                print(f"modify failed, please check cs file name: {file}")
                return

            proto_file_name = f"{message_name}_{start_id}.proto"
            proto_file_path = os.path.join("./", proto_file_name)
            if not os.path.exists(proto_file_path):
                print(f"modify failed, please check proto file name: {proto_file_name}")
            # 打开proto文件
            proto_lines = None
            with open (proto_file_path, "r", encoding="utf-8") as f:
                proto_lines = f.readlines()

            # 储存所有协议的附加注释信息(返回类型 和 消息类型)
            proto_comments = {}
            iterator = iter(proto_lines)
            for it in iterator:
                match_res_type = re.match(proto_res_type_pattern, it)
                if match_res_type:
                    res_type = match_res_type.group(1)
                    next_line = next(iterator)
                    match_msg_type = re.match(proto_msg_type_pattern, next_line)
                    if not match_msg_type:
                        print(f"modify failed, 注解了返回类型，但是没有注解消息类型 please check proto file: {proto_file_name}, line: {it}")
                        return
                    msg_name = match_msg_type.group(1)
                    msg_type = match_msg_type.group(2)
                    if msg_type != "req":
                        print(f"modify failed, 注解了返回类型，但是注解的消息类型不是 req, please check proto file: {proto_file_name}, line: {it}")
                        return
                    proto_comments[msg_name] = {
                        "res_type": res_type,
                        "msg_type": msg_type,
                        "msg_id": 0,
                    }
                    continue

                match_msg_type = re.match(proto_msg_type_pattern, it)
                if match_msg_type:
                    msg_name = match_msg_type.group(1)
                    msg_type = match_msg_type.group(2)
                    proto_comments[msg_name] = {
                        "msg_type": msg_type,
                        "msg_id": 0,
                    }
                    continue

                # 普通消息全认为是 msg
                match_msg = re.match(proto_msg_pattern, it)
                if match_msg:
                    msg_name = match_msg.group(1)
                    proto_comments[msg_name] = {
                        "msg_type": "msg",
                        "msg_id": 0,
                    }

            file_path = os.path.join(root, file)
            with open(file_path, "r", encoding="utf-8") as f:
                lines = f.readlines()

            # 写 opcode
            opcode_contents = ""
            for msg_name, info in proto_comments.items():
                start_id = start_id + 1
                info["msg_id"] = start_id
                opcode_contents += opcode_item.format(msg_name, info["msg_id"])
            opcode_block = opcode_class.format(message_name, opcode_contents)

            # 修改协议
            with open(file_path, "w", encoding="utf-8") as f:
                for line in lines:
                    ns_match = re.match(cs_namespace_pattern, line)
                    if ns_match:
                        f.write(line)
                        f.write(opcode_block)
                        continue

                    class_match = re.match(cs_class_pattern, line)
                    if class_match:
                        class_name = class_match.group(1)
                        comment_info = proto_comments[class_name]
                        if comment_info is None:
                            print(f"modify failed, proto file: {proto_file_name}, class: {class_name}'s comment not found")
                            return

                        f.write(f"[Message({message_name}.{class_name})]\n")
                        msg_type = comment_info["msg_type"]
                        if msg_type == "req":
                            res_type = comment_info["res_type"]
                            f.write(f"ResponseType(nameof({res_type}))\n")

                        interface_type = "IMessage"
                        if msg_type == "req":
                            interface_type = "IRequest"
                        if msg_type == "res":
                            interface_type = "IResponse"
                        line = f"public sealed partial class {class_name} : MessageObject, {interface_type}, pb::IMessage<{class_name}>\n"
                        f.write(line)
                        continue
                    f.write(line)

    print(f"finish modify {language} =========================")


if __name__ == '__main__':
    modify("csharp")