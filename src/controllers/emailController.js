const emailService = require('../services/emailService');

const sendTestEmail = async (req, res) => {
    const { email, name, language } = req.body;
    try {
        await emailService.sendEmail(email, name, language);
        res.status(200).send('Email sent successfully');
    } catch (error) {
        console.error(error);
        res.status(500).send('Failed to send email');
    }
};

module.exports = {
    sendTestEmail
};
