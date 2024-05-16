const nodemailer = require('nodemailer');
const config = require('config');
const {generatePDF} = require("./pdfGenerationService");
const fs = require('fs');

const transporter = nodemailer.createTransport({
    service: 'gmail',
    host: 'smtp.gmail.com',
    port: 465,
    secure: true,
    auth: {
        user: config.get('email.user'),
        pass: config.get('email.pass')
    }
});

const sendEmail = async (email, name, language) => {
    const mailOptions = {
        from: config.get('email.user'),
        to: email,
        subject: "Test Email",
        text: "This is a test email",
    };

    // generate PDF file
    const pdfFilePath = `${email}.pdf`;
    await generatePDF(pdfFilePath, name, language); // 生成 PDF 文件

    // attach the PDF file to the email
    const attachment = {
        filename: `Diploma_${name}.pdf`,
        path: pdfFilePath
    };
    mailOptions.attachments = [attachment];


    try {
        const info = await transporter.sendMail(mailOptions);
        console.log('Email sent: ' + info.response);
    } catch (error) {
        console.error('Error sending email: ', error);
        throw error;
    } finally {
        // delete the PDF file whether sending the email
        try {
            fs.unlinkSync(pdfFilePath);
            console.log('PDF file deleted successfully');
        } catch (err) {
            console.error('Error deleting PDF file: ', err);
        }
    }
};

module.exports = {
    sendEmail
};
