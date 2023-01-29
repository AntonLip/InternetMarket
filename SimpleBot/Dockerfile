FROM python:3.8 AS builder
COPY [".", "."]
#WORKDIR /SimplBot
# установка зависимостей в локальную директорию user (например, /root/.local)
RUN pip install -r requirements.txt
# второй этап (без названия)
#WORKDIR /code
# копирование только установки зависимостей из образа первого этапа
#COPY --from=builder /root/.local/bin /root/.local
#COPY ["SimplBot/simpleBot.py", "."]
# обновление переменной среды PATH
ENV PATH=/root/.local:$PATH
CMD [ "python3", "simpleBot.py", "http://webapp", "1", "3" ]
