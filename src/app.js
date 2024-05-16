const express = require('express');
const bodyParser = require('body-parser');
const emailRoutes = require('./routes/emailRoute');

const app = express();

app.use(bodyParser.json());
app.use('/email', emailRoutes);

module.exports = app;
