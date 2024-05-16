const PDFDocument = require('pdfkit');
const fs = require('fs');

// 生成 PDF 函数
const generatePDF = async (filePath, name, language) => {
    const doc = new PDFDocument();
    const stream = fs.createWriteStream(filePath);

    // 将 PDF 文件保存到本地
    doc.pipe(stream);

    // TODO add name
    doc.fontSize(12).text(name, { align: 'left' });

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
