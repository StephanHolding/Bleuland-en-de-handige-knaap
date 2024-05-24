const PDFDocument = require('pdfkit');
const fs = require('fs');

// 生成 PDF 函数
const generatePDF = async (filePath, name, language) => {
    const doc = new PDFDocument();
    const stream = fs.createWriteStream(filePath);

    // 将 PDF 文件保存到本地
    doc.pipe(stream);

    // 添加标题图片
    doc.image('header.png', {
        fit: [500, 250], // 图片尺寸调整为适合的大小
        align: 'center', // 图片居中
        valign: 'top'    // 图片位于页面顶部
    });
    // 留出空白空间
    doc.moveDown(20);
    // 添加姓名到 PDF
    doc.fontSize(20).text(`Internship Certificate`, {align: 'center'}).moveDown();
    doc.moveDown(2);

    doc.fontSize(12).text(`This is to certify that`, {align: 'center'}).moveDown();
    doc.fontSize(12).text(name, { align: 'center', oblique: true, bold: true }).moveDown();
    doc.fontSize(12).text(`has successfully completed an internship at Utrecht University.`, {align: 'center'}).moveDown();

    doc.moveDown(5);
    doc.fontSize(12).text(`Date: ${new Date().toDateString()}`, {align: 'right'}).moveDown();
    doc.fontSize(12).text(`Jan Bleuland`, {align: 'right'});
    doc.fontSize(12).text(`Rector Magnificus, Utrecht University`, {align: 'right'});

    // 结束并保存 PDF
    doc.end();

    // 返回 Promise
    return new Promise((resolve, reject) => {
        stream.on('finish', resolve);
        stream.on('error', reject);
    });
};

module.exports = {
    generatePDF
};
