const express = require('express');
const router = express.Router();
const emailController = require('../controllers/emailController');
const authenticateToken = require("../middleware/authenticator");

router.post('/pdf', authenticateToken, emailController.sendTestEmail);

module.exports = router;
