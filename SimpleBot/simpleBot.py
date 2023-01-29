import requests
from bs4 import BeautifulSoup as bs
import time
import sys


sc = sys.argv[0]
#Адрес главной страницы
#url = 'http://192.168.5.90:8080'
url = str(sys.argv[1])

#Функция запроса 
def get(urls):
    return requests.get(urls)


#Просмотреть request-заголовки 
for key, value in get(url).request.headers.items():
    print(key+": "+value)


#Найдем все ссылки на странице
links = bs(get(url).text, "html.parser").find_all('a')

#Сформируем список адресов всех ссылок для дальнейшей работы
urls = [url + str(l.attrs['href']) for l in links]

#Запросы по ссылкам каждые 10 секунд
c=0
while c<int(sys.argv[2]):
 start = time.time()
 for i in urls:
     start1 = time.time()
     get(i)
     #print(get(i).status_code) #Просмотреть статус подключения
     print(f'by_one_link: {time.time() - start1 : .2f} seconds')
 print(f'by_all_link: {time.time() - start : .2f} seconds')
 time.sleep(float(sys.argv[3]))
 print(f'all: {time.time() - start : .2f} seconds')
 c=c+1
