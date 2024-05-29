require('dotenv').config();
const config = require('config');
const app = require('./app');

const PORT = config.get('port') || process.env.PORT;

app.listen(PORT, () => {
    console.log(`Server is running on port ${PORT}`);
});
