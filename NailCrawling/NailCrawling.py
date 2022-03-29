from urllib import request
from selenium.webdriver.common.keys import Keys
from selenium import webdriver
import os
import asyncio
import urllib.request
import time

def createDirectory(directory):
    try:
        if not os.path.exists(directory):
            os.makedirs(directory)
    except OSError:
        print("Error: Failed to create the directory.")

def googleImgEnter(driver,imgURL):
    driver.find_element_by_xpath('//*[@id="sbtc"]/div/div[3]/div[2]').click()
    driver.find_element_by_xpath('//*[@id="Ycyxxc"]').send_keys(imgURL)
    driver.find_element_by_xpath('//*[@id="RZJ9Ub"]').click()
    driver.find_element_by_xpath('//*[@id="rso"]/div[2]/div/div[2]/g-section-with-header/div[1]/title-with-lhs-icon/a').click()
    return
def imgCrawling(driver,dirNumber,label):
    SCROLL_PAUSE_TIME = 1
    # Get scroll height
    last_height = driver.execute_script("return document.body.scrollHeight")  # 브라우저의 높이를 자바스크립트로 찾음
    while True:
        # Scroll down to bottom
        driver.execute_script("window.scrollTo(0, document.body.scrollHeight);")  # 브라우저 끝까지 스크롤을 내림
        # Wait to load page
        time.sleep(SCROLL_PAUSE_TIME)
        # Calculate new scroll height and compare with last scroll height
        new_height = driver.execute_script("return document.body.scrollHeight")
        if new_height == last_height:
            try:
                driver.find_element_by_css_selector(".mye4qd").click()
            except:
                break
        last_height = new_height
    imgs = driver.find_elements_by_css_selector(".rg_i.Q4LuWd")
    dir = "./DataSet/"+label+"/url"+str(dirNumber)
    createDirectory(dir) #폴더 생성
    Max=250
    for i,img in enumerate(imgs):
        try:
            img.click()
            time.sleep(1)
            imgUrl = driver.find_element_by_xpath(
            '//*[@id="Sva75c"]/div/div/div[3]/div[2]/c-wiz/div/div[1]/div[1]/div[2]/div/a/img').get_attribute(
            "src")
            if imgUrl[:5]=="https":
                path = "C:\\Users\\GVR_LAB\\Desktop\\NailCrawling\\"+dir+"\\"
                urllib.request.urlretrieve(imgUrl, path+str(i)+".jpg")
            else: i-=1
        except:
            i-=1
            pass
        if i>Max: return
def RunURLs(imgURLs,label=None):
    label,imgURLs=list(imgURLs.items())[0]
    for i,imgURL in enumerate(imgURLs):
        url="https://www.google.co.kr/imghp?hl=en&ogbl"
        options = webdriver.ChromeOptions()
        driver = webdriver.Chrome('chromedriver.exe', options=options)
        driver.implicitly_wait(100)
        driver.get(url)
        googleImgEnter(driver,imgURL)
        imgCrawling(driver,i,label)
        driver.close()
def getURLs():
    import json
    import os
    fileLocation="imgURL.json"
    file=open(f"{os.path.dirname(os.path.realpath(__file__))}\{fileLocation}")
    URLs=json.load(file)["urls"]
    return URLs

RunURLs(getURLs())
