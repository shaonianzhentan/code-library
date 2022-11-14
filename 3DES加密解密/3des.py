# pip install pyDes
import pyDes, base64, hashlib

def md5(data):
    return hashlib.md5(data.encode('utf-8')).hexdigest()

class EncryptHelper:

    def __init__(self, key, iv) -> None:
        key = md5(key)
        iv = md5(iv)[0:11] + '='
        self.key = base64.decodebytes(key.encode('utf-8'))
        self.iv = base64.decodebytes(iv.encode('utf-8'))

    def Encrypt(self, data):
        k = pyDes.triple_des(self.key, pyDes.CBC, padmode=pyDes.PAD_PKCS5, IV=self.iv)
        d = k.encrypt(data)
        return str(base64.b64encode(d), encoding="utf-8")

    def Decrypt(self, data):
        k = pyDes.triple_des(self.key, pyDes.CBC, padmode=pyDes.PAD_PKCS5, IV=self.iv)        
        d = k.decrypt(base64.b64decode(data))
        return d.decode()

helper = EncryptHelper("123456", "111")
oldValue = "13800138000"
newValue = helper.Encrypt(oldValue)
print("加密：" + newValue)
print("解密：" + helper.Decrypt(newValue))