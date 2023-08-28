FROM python:3.7-slim
WORKDIR /app
COPY . /app
RUN pip install --trusted-host pypi.python.org -r "requirements.txt"
EXPOSE 8082
ENV NAME Gidget
CMD ["python3.7", "app2.py"]
