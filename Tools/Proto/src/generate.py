import os
import shutil
import subprocess

import config


def generate(language):
    print(f"start generate {language} =========================")

    output_path = config.output_path[language]
    if output_path is None:
        print(f"{language} not support")
        return

    shutil.rmtree(output_path, ignore_errors=True)
    os.mkdir(output_path)

    # 遍历当前目录下的所有文件
    for root, _, files in os.walk("."):
        for file in files:
            if not file.endswith(".proto"):
                continue

            cmd = ["protoc", "-I=.", f"--{language}_out={output_path}", file]
            result = subprocess.run(cmd, capture_output=True, text=True)
            if result.returncode != 0:
                print(f"{file} generate error: {result.stderr}")
                return
            else:
                print(f"{file} generate success")

        break

    print(f"finish generate {language} =========================")


if __name__ == '__main__':
    generate("csharp")

