import os
import re

import config

# cs 类名称
cs_class_pattern = r'public sealed partial class (\w+)\s+:\s+pb::IMessage<'
# cs 文件名称
cs_file_pattern = r'(\w+)(\d+).cs'

# proto 返回类型  // ResponseType ObjectQueryResponse
proto_res_type_pattern = r'//\s+ResponseType\s+(\w+)'
# proto 消息类型   // IRequest
proto_msg_type_pattern = r'\s*message\s+(\w+)\s+//\s*(\w+)'



def modify(language):
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
            start_id = ""
            if match_result:
                message_name = match_result.group(1)
                start_id = match_result.group(2)
            else:
                print(f"modify failed, please check cs file name: {file}")
                return

            proto_file_name = f"{message_name}_{start_id}.proto"
            proto_file_path = os.path.join("./", proto_file_name)
            if not os.path.exists(proto_file_path):
                print(f"modify failed, please check proto file name: {proto_file_name}")
            # 打开proto文件
            with open (proto_file_path, "r", encoding="utf-8") as f:
                pass



            # 储存所有协议名称
            proto_names = []

            file_path = os.path.join(root, file)
            with open(file_path, "r", encoding="utf-8") as f:
                lines = f.readlines()

            for line in lines:
                pass























if __name__ == '__main__':
    modify("csharp")