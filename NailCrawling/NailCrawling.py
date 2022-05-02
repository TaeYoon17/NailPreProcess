from urllib import request
from selenium.webdriver.common.keys import Keys
from selenium import webdriver
from selenium.webdriver.common.by import By
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

def GoogleImgEnter(driver,imgURL):
    driver.find_element_by_xpath('//*[@id="sbtc"]/div/div[3]/div[2]').click()
    driver.find_element_by_xpath('//*[@id="Ycyxxc"]').send_keys(imgURL)
    driver.find_element_by_xpath('//*[@id="RZJ9Ub"]').click()
    driver.find_element_by_xpath('//*[@id="rso"]/div[2]/div/div[2]/g-section-with-header/div[1]/title-with-lhs-icon/a').click()
    return

def imgsSelector(driver):
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
    return imgs

def imgDownload(driver,dirNumber,label,imgs):
    dir = "C:\\Users\\MY_PC\\Desktop\\DataSetCrawl\\RawDataSet\\"+label+"/url"+str(dirNumber)
    createDirectory(dir) #폴더 생성
    Max=250
    for i,img in enumerate(imgs):
        try:
            img.click()
            time.sleep(1)
            imgUrl=driver.find_element(By.XPATH,
                '/html/body/div[3]/c-wiz/div[3]/div[2]/div[3]/div/div/div[3]/div[2]/c-wiz/div/div[1]/div[1]/div[3]/div/a/img').get_attribute("src")
            if imgUrl[:5]=="https":
                path = dir+"\\"
                urllib.request.urlretrieve(imgUrl, path+str(i)+".jpg")
                print(f"Is Downloaded {i}")
            else: 
                i-=1
                print("Not Downloaded")
        except:
            print("imgURL Error!!")
            i-=1
            pass
        if i>Max: return

def ImgCrawling(driver,i,label):
    imgs=imgsSelector(driver)
    imgDownload(driver,i,label,imgs)

def RunURLs(imgURLs,label=None):
    label,imgURLs=list(imgURLs.items())[0]
    for i,imgURL in enumerate(imgURLs):
        url="https://www.google.co.kr/imghp?hl=en&ogbl"
        options = webdriver.ChromeOptions()
        driver = webdriver.Chrome('chromedriver.exe', options=options)
        driver.implicitly_wait(100)
        driver.get(url)
        GoogleImgEnter(driver,imgURL)
        ImgCrawling(driver,i,label)
        driver.close()
def getURLs():
    import json
    import os
    fileLocation="imgURL.json"
    file=open(f"{os.path.dirname(os.path.realpath(__file__))}\{fileLocation}")
    URLs=json.load(file)["urls"]
    return URLs

RunURLs(getURLs())
