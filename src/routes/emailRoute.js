const express = require('express');
const router = express.Router();
const emailController = require('../controllers/emailController');

router.post('/pdf', emailController.sendTestEmail);

module.exports = router;
