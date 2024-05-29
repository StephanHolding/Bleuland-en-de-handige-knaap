const jwt = require('jsonwebtoken');
const secretKey = 'HKU123'; // 这里使用你的密钥

// JWT 验证中间件
function authenticateToken(req, res, next) {
    const token = req.headers.authorization && req.headers.authorization.split(' ')[1];

    if (!token) {
        return res.status(401).json({ message: 'Unauthorized' });
    }

    try {
        // 验证 JWT
        jwt.verify(token, secretKey);

        next(); // 继续处理下一个中间件或路由处理器
    } catch (error) {
        return res.status(401).json({ message: 'Unauthorized' });
    }
}

module.exports = authenticateToken;
