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
    const mailSubjectEng = "Anatomy Adventures and an Internship Certificate";
    const mailTextEng = ` 
Dear ${name},

Thank you for being the unsung hero in our anatomical escapades! Your assistance has been nothing short of exceptional, and I can't help but wonder if you have a hidden talent for dissecting tricky situations, both literally and figuratively.

To express my gratitude, I am delighted to provide you with an internship certificate, confirming your invaluable contributions to our work at Utrecht University.

May your future endeavors be as precise and insightful as your help has been to me. Keep up the fantastic work!

Best regards,
Jan Bleuland
Rector Magnificus, Utrecht University
`;
    const mailSubjectNl = "Anatomische Avonturen en een Certificaat";
    const mailTextNl =  ` 
Beste ${name},

Bedankt voor uw broodnodige hulp met het afmaken van het anatomische model! Uw hulp was uitzonderlijk geweldig, ik vraag me af of u misschien een verbogen talent hebt voor het ontleden van lastige situaties; zowel letterlijk als figuurlijk.

Om mijn dankbaarheid te uiten, overhandig ik u met veel genoegen een certificaat, waarmee uw waardevolle bijdrage aan ons werk aan de Universiteit Utrecht wordt bevestigd.

Mogen uw toekomstige inspanningen net zo nauwkeurig en inzichtelijk zijn als dat het voor mij is geweest. Ga zo door met het fantastische werk!

Vriendelijke groet,
Jan Bleuland
Rector Magnificus, Universiteit Utrecht
`;
    const mailOptions = {
        from: {
            name: 'Jan Bleuland',
            address: config.get('email.user')
        },
        to: email,
        subject: language === 'en' ? mailSubjectEng : mailSubjectNl,
        text: language === 'en' ? mailTextEng : mailTextNl,
    };

    // generate PDF file
    const pdfFilePath = `${email}_${new Date().getTime()}.pdf`;
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
            console.log(`PDF file deleted successfully ${pdfFilePath}`);
        } catch (err) {
            console.error('Error deleting PDF file: ', err);
        }
    }
};

module.exports = {
    sendEmail
};
