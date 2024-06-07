const PDFDocument = require('pdfkit');
const fs = require('fs');
const sizeOf = require('image-size');

// 生成 PDF 函数
const generatePDF = async (filePath, name, language) => {
    let imagePath = language === 'en' ? `en-template.png` : `nl-template.png`;
    // 获取图片的尺寸
    const dimensions = sizeOf(imagePath);
    const { width, height } = dimensions;

    const doc = new PDFDocument({
        size: [width, height] // 根据图片的尺寸设置PDF页面大小
    });
    const stream = fs.createWriteStream(filePath);

    // 将 PDF 文件保存到本地
    doc.pipe(stream);

    // 将图片占满整个页面
    doc.image(imagePath, 0, 0, {
        width: width,
        height: height
    });
    // 设置字体
    doc.registerFont('CursiveFont', 'Monotype Corsiva.ttf');
    doc.fontSize(48)
        .font('CursiveFont') // 设置字体为 Helvetica 斜体加粗
        .fillColor('#595959'); // 设置字体颜色

    // 计算文本的位置
    const textWidth = doc.widthOfString(name);
    const textHeight = doc.heightOfString(name);
    const x = (width - textWidth) / 2;
    const y = (height - textHeight) / 2 - 20;

    // 添加文字到图片上
    doc.text(name, x, y);

    //添加日期
    const date = new Date();
    const day = date.getDate();
    const locale = language === 'en' ? 'en-US' : 'nl-NL';
    const month = date.toLocaleString(locale, { month: 'long' });
    const year = date.getFullYear();
    const dateString = `${day} ${month} ${year}`;
    const dateWidth = doc.widthOfString(dateString);
    const dateHeight = doc.heightOfString(dateString);
    const x_date = (width - dateWidth) / 2;
    const y_date = (height - dateHeight) / 2 - 20;
    doc.fontSize(32).text(dateString, x_date + 105, y_date + 128);

    // 结束文档
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
