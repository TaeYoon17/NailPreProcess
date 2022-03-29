
import urllib.request
def getURLs():
    import json
    import os
    fileLocation="imgURL.json"
    file=open(f"{os.path.dirname(os.path.realpath(__file__))}\{fileLocation}")
    URLs=json.load(file)["urls"]
    return URLs

imgUrl="https://haileyfashionlife.com/wp-content/uploads/2022/01/30-Cute-valentines-nails-with-nails-art-ideas-2022-8.jpg"
def createDirectory(directory):
    import os
    try:
        if not os.path.exists(directory):
            os.makedirs(directory)
    except OSError:
        print("Error: Failed to create the directory.")

dir = "./DataSet/url"+str(2)
path = "C:\\Users\\MY_PC\\Desktop\\NailCrawling\\"+dir+"\\"
createDirectory(dir)
urllib.request.urlretrieve(imgUrl, path+str(1)+".jpg")