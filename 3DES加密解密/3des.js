// npm install crypto-js
import CryptoJS from 'crypto-js'

export default class EncryptHelper {
    constructor(key, iv) {
        key = this.md5(key)
        iv = this.md5(iv).substring(0, 11) + '='
        this.key = CryptoJS.enc.Base64.parse(key);
        this.iv = CryptoJS.enc.Base64.parse(iv);
    }

    Encrypt(data) {
        const encrypted = CryptoJS.TripleDES.encrypt(data, this.key, {
            iv: this.iv,
            mode: CryptoJS.mode.CBC,
            padding: CryptoJS.pad.Pkcs7
        });
        return encrypted.toString();
    }

    Decrypt(data) {
        const decrypted = CryptoJS.TripleDES.decrypt(data, this.key, {
            iv: this.iv,
            mode: CryptoJS.mode.CBC,
            padding: CryptoJS.pad.Pkcs7
        }).toString(CryptoJS.enc.Utf8);
        return decrypted
    }

    md5(data) {
        return CryptoJS.MD5(data).toString()
    }
}

const helper = new EncryptHelper("123456", "111")
var oldValue = "13800138000";
var newValue = helper.Encrypt(oldValue);
console.log("加密：" + newValue);
console.log("解密：" + helper.Decrypt(newValue));