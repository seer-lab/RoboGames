const nodemailer = require('nodemailer');
const config = require('../config.json');

var transporter = nodemailer.createTransport({
    service: 'gmail',
    auth: {
      user: config.email,
      pass: config.password
    }
});

function sendEmail(subject, message){
    var mailOptions = {
        from: config.email,
        to: config.email2,
        subject: subject,
        text: message
    };

    transporter.sendMail(mailOptions, function(err, info){
        if(err){
          console.log(err);
        }else{
          console.log("Email Sent"+ info.response);
        }
    });
}

sendEmail("Unity Compilation Notice!", "Finished Compilation at " + Date());