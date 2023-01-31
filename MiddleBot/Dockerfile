FROM python:alpine AS builder
COPY [".", "."]
RUN pip install -r requirements.txt

CMD [ "python3", "middlebot.py", "http://webapp", "1", "3" ]
