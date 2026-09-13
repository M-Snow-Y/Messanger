# 💬 Messenger (.NET MAUI Blazor Hybrid + ASP.NET Core)

<p align="center">
  <img src="https://img.shields.io/badge/.NET-8.0-512BD4?style=for-the-badge&logo=dotnet&logoColor=white" alt=".NET 8" />
  <img src="https://img.shields.io/badge/C%23-239120?style=for-the-badge&logo=c-sharp&logoColor=white" alt="C#" />
  <img src="https://img.shields.io/badge/Blazor-Hybrid-512BD4?style=for-the-badge&logo=blazor&logoColor=white" alt="Blazor" />
  <img src="https://img.shields.io/badge/ASP.NET_Core-Web_API-5C2D91?style=for-the-badge&logo=.net&logoColor=white" alt="ASP.NET Core" />
  <img src="https://img.shields.io/badge/Microsoft_SQL_Server-CC292B?style=for-the-badge&logo=microsoft-sql-server&logoColor=white" alt="MSSQL" />
</p>

---

## 🚀 Основные возможности

- 🔐 **Аутентификация и безопасность:**
  - Регистрация и авторизация пользователей по номеру телефона и паролю.
  - Криптографическое хэширование паролей (SHA-256) на стороне клиента и валидация на сервере.
  - Сохранение сессии в нативных защищенных хранилищах (`Preferences`).

- 💬 **Диалоги и чаты:**
  - **Личные сообщения (1-на-1):** безопасный обмен сообщениями между двумя пользователями с автоматическим созданием приватной комнаты диалога.
  - **Групповые чаты:** поддержка создания групп и добавления участников.
  - Отображение статусов собеседников и времени отправки сообщений.

- 🖼️ **Медиа и файлы:**
  - Прикрепление и передача изображений в сообщениях.
  - Встроенный полноэкранный просмотр фото (**Lightbox**) с затемнением фона и анимацией.

- 🎨 **Интерфейс в стиле Telegram Dark Theme:**
  - Полноценная тёмная тема с аутентичной палитрой цветов Telegram Desktop.
  - Выдвижное боковое меню (`<nav>`) профиля с аватаром, статусом и быстрыми действиями.
  - Полная адаптивность: мобильный режим с переключением между сайдбаром и областью активного диалога.
  - Изоляция стилей (Scoped CSS) компонентов Blazor.

---

## 🛠️ Стек технологий и архитектура

Проект построен по классической трехзвенной архитектуре с разделением ответственности:

Backend (Сервер)
ASP.NET Core Web API (.NET 8) — высокопроизводительный бэкенд.
Entity Framework Core 8 — ORM для работы с базой данных (Code First / Database First).
Microsoft SQL Server (SSMS) — реляционная база данных для персистентного хранения пользователей, чатов и истории сообщений.
RESTful API — структурированные контроллеры с применением паттерна DTO (Data Transfer Object).

Frontend (Клиент)
.NET MAUI Blazor Hybrid — запуск веб-компонентов внутри нативного контейнера операционной системы.
Blazor Component Model (Razor) — реактивный интерфейс на C# без необходимости писать на JavaScript.
Scoped CSS — компонентная изоляция стилей интерфейса.
  📋 Модели данных (ER-структура)
User — сущность пользователя (ID, номер телефона, никнейм, хэш пароля, дата регистрации).
Chat — сущность чата (ID, название, флаг IsGroup, список участников Users, сообщения Messages).
Message — сущность сообщения (ID, текст, ссылка на изображение/файл, время отправки, автор SenderId, чат ChatId).
PrivateChatRequest — DTO-модель запроса создания личного диалога между пользователями.
  ⚙️ Установка и запуск проекта
Предварительные требования
Visual Studio 2022 (версии 17.8+) с установленными рабочими нагрузками:
Разработка мобильных приложений на .NET (.NET MAUI)
ASP.NET и разработка веб-приложений
Microsoft SQL Server и SSMS (SQL Server Management Studio).
.NET 8 SDK.
  1. Клонирование репозитория
code
Bash
git clone https://github.com/M-Snow-Y/Messanger.git
cd Messanger
  2. Настройка базы данных
В файле Messanger.Server/appsettings.json укажите вашу строку подключения к SQL Server:
code
JSON
"ConnectionStrings": {
  "DefaultConnection": "Server=localhost;Database=MessangerDb;Trusted_Connection=True;TrustServerCertificate=True;"
}
Примените миграции (если используются):
code
Bash
dotnet ef database update --project Messanger.Server
  3. Запуск сервера
code
Bash
cd Messanger.Server
dotnet run
Сервер будет доступен по адресу: http://localhost:5050.
  4. Запуск клиента
В Visual Studio выберите проект Messanger.Client в качестве запускаемого, выберите целевую платформу (Windows Machine или эмулятор Android) и нажмите F5 (Запуск).



👨‍💻 Автор
Разработчик: M-Snow-Y
