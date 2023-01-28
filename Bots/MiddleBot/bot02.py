import requests
from bs4 import BeautifulSoup as bs
import multiprocessing as mp
import time
import threading

a=1
b=1

#Адрес главной страницы
url = 'http://localhost:5000'

#Функция запроса 
def get(urls):
    return requests.get(urls)


#Просмотреть request-заголовки 
for key, value in get(url).request.headers.items():
    print(key+": "+value)


'''
#Проверим подключение
req = get(url)
print(' ')
print(req.status_code)
print(req.text)  #Вывод html-структуры сайта
print(' ')
'''

#Найдем все ссылки на странице
links = bs(get(url).text, "html.parser").find_all('a')

'''
#Выведем все ссылки на странице
print(' ')
print('--------Имеющиеся ссылки------------')
for l in links: 
    print(l.attrs['href'])
print('----------Конец списка--------------')
print(' ')
'''
#Сформируем список адресов всех ссылок для дальнейшей работы
urls = [url + str(l.attrs['href']) for l in links]
'''
#Если надо вывести список
print(' ')
print('--------Адреса всех ссылок------------')
print(' ')
print(urls) 
print('------------Конец списка--------------')
print(' ')
'''

#Запросы по ссылкам каждые 10 секунд
c=0
while c<int(a):
 start = time.time()
 for i in urls:
     start1 = time.time()
     get(i)
     #print(get(i).status_code) #Просмотреть статус подключения
     print(f'by_one_link: {time.time() - start1 : .2f} seconds')
 print(f'by_all_link: {time.time() - start : .2f} seconds')
 time.sleep(float(b))
 print(f'all: {time.time() - start : .2f} seconds')
 c=c+1
