# 使用官方的 Node.js 18 运行时镜像作为基础镜像
FROM node:18.17.1

# 创建并设置工作目录
WORKDIR /app

# 复制 package.json 和 package-lock.json
COPY package*.json ./

# 安装项目依赖
RUN npm install

# 复制项目文件到工作目录
COPY . .

# 暴露应用程序运行的端口
EXPOSE 3000

# 运行应用程序
CMD ["npm", "start"]
