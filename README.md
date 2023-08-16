Чат-бот состоит из нескольких архитектурных компонентов

Платежного шлюза, интегрированного посредством HTTP API.

Сервера на базе linux с развернутой документоориентированной СУБД mongodb, что позволяет гибко управлять сетевой реферальной моделью подписки.

сервера на базе linux с контейнеризированным веб-приложением на языке c#, реализующим бизнес-логику бота.

Телеграм-интерфейса, обеспечивающим интерфейсное взаимодействие с пользователем.

```
sequenceDiagram
autonumber

Actor user
participant telegram
participant bot
participant DB

user  -->>  telegram : интерфейсное взаимодействие <br/> (http, клиент)
telegram -->> bot :API <br/> telegram
bot -->> DB : db <br/>connection
bot -->> wallet : http api
bot -->> telegram: 
telegram -->> user: 

```
