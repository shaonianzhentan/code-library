const crypto = require('crypto')

module.exports = class EncryptHelper {
    constructor(key, iv) {
        key = this.md5(key)
        iv = this.md5(iv).substring(0, 11) + '='
        this.key = Buffer.from(key, "base64")
        this.iv = Buffer.from(iv, "base64")
    }

    Encrypt(data) {
        var cipher = crypto.createCipheriv('des-ede3-cbc', this.key, this.iv);
        var ciph = cipher.update(data, 'utf8', 'base64');
        ciph += cipher.final('base64');
        return ciph
    }

    Decrypt(data) {
        var decipher = crypto.createDecipheriv('des-ede3-cbc', this.key, this.iv);
        var txt = decipher.update(data, 'base64', 'utf8');
        txt += decipher.final('utf8');
        return txt
    }

    md5(data) {
        return crypto.createHash('md5').update(data).digest('hex')
    }
}

const helper = new EncryptHelper("123456", "111")
var oldValue = "13800138000";
var newValue = helper.Encrypt(oldValue);
console.log("加密：" + newValue);
console.log("解密：" + helper.Decrypt(newValue));