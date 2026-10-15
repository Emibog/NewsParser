# 📰 NewsParser - Telegram Бот для Парсинга Новостей

[![.NET](https://img.shields.io/badge/.NET-10.0-blue.svg)](https://dotnet.microsoft.com/)
[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](https://opensource.org/licenses/MIT)

**NewsParser** — это автоматизированный бот для Telegram, который мониторит RSS-ленты новостных сайтов и отправляет вам только релевантные новости в выбранный чат.

## ✨ Основные возможности

- 🔄 **Автоматический мониторинг** RSS-лент с заданной периодичностью
- 🔍 **Умная фильтрация** по ключевым словам и стоп-словам
- 📱 **Интеграция с Telegram Bot API** для мгновенных уведомлений
- 💾 **SQLite база данных** для хранения уже просмотренных новостей
- ⚙️ **Настройка через JSON** конфигурационный файл

## 🏗 Архитектура проекта

```
NewsParser/
├── Model/
│   └── NewsItem.cs           # Модель новости (Title, Link, PublishDate)
├── Service/
│   ├── NewsWorker.cs         # Основной рабочий процесс (BackgroundService)
│   ├── NewsParser.cs         # Парсинг RSS-лент
│   ├── NewsFilter.cs         # Фильтрация новостей по ключевым словам
│   ├── NewsRepository.cs     # Работа с базой данных SQLite
│   └── TelegramNotifier.cs   # Отправка уведомлений в Telegram
├── DB/
│   └── news.db               # SQLite база данных (автосоздаётся)
├── appsettings.json          # Конфигурация приложения
└── Program.cs                # Точка входа
```

## 🚀 Быстрый старт

### Требования

- [.NET 10.0 SDK](https://dotnet.microsoft.com/download/dotnet/10.0)
- [Telegram Bot Token](https://core.telegram.org/bots#6-botfather)
- ID целевого чата в Telegram

### Установка

1. **Клонируйте репозиторий:**
   ```bash
   git clone <repository-url>
   cd NewsParser
   ```

2. **Настройте конфигурацию** (`appsettings.json`):
   
   ```json
   {
     "NewsSettings": {
       "FeedUrls": [
         "https://lenta.ru/rss/news",
         "https://habr.com/ru/rss/all/all/",
         "https://feeds.npr.org/1001/rss.xml"
       ],
       "Keywords": [
         "Яндекс",
         "Gemini",
         "qwen",
         "Ollama",
         "LM studio",
         "KDE plasma",
         "plasma",
         "steam",
         "атак"
       ],
       "StopWords": [
         "реклама",
         "промокод",
         "криптовалюта"
       ],
       "CheckIntervalMinutes": 5
     },
     "TelegramSettings": {
       "BotToken": "ВАШ_BOT_TOKEN",
       "ChatId": ВАШ_CHAT_ID
     }
   }
   ```

3. **Соберите проект:**
   ```bash
   dotnet build
   ```

4. **Запустите бота:**
   ```bash
   dotnet run
   ```

## ⚙️ Конфигурация

### NewsSettings

| Параметр | Тип | Описание |
|----------|------|----------|
| `FeedUrls` | `string[]` | Список URL RSS-лент для мониторинга |
| `FeedUrlsForAllNews` | `string[]` | Список URL RSS-лент которые не проходят фильтрацию по ключевым словам (будет откправлять все новости) |
| `Keywords` | `string[]` | Ключевые слова для фильтрации (по умолчанию: все новости) |
| `StopWords` | `string[]` | Слова, которые исключают новость из рассылки |
| `CheckIntervalMinutes` | `int` | Интервал проверки RSS-лент в минутах |

### TelegramSettings

| Параметр | Тип | Описание |
|----------|------|----------|
| `BotToken` | `string` | Token бота от @BotFather |
| `ChatId` | `long` | ID целевого чата (личный или групповой) |

## 📝 Как получить ChatId

1. Отправьте сообщение боту в Telegram
2. Используйте [@get_my_id](https://t.me/getmyidbot) бота для получения вашего ChatId
3. Или используйте [Telegraf.io](https://telegra.ph/Get-My-Id-Bot-04-09)

## 🔧 Разработка

### Добавление новой RSS-ленты

Добавьте URL в массив `FeedUrls` в `appsettings.json`:

```json
"FeedUrls": [
  "https://example.com/rss",
  "https://another-news.com/feed"
]
```

### Настройка ключевых слов

Измените `Keywords`, чтобы бот отправлял только новости по интересующим темам:

```json
"Keywords": [
  "Искусственный интеллект",
  ".NET",
  "программирование"
]
```

## 📦 Структура данных

### NewsItem

```csharp
public class NewsItem
{
    public string Title { get; set; }      // Заголовок новости
    public string Link { get; set; }       // Ссылка на новость
    public DateTimeOffset PublishDate { get; set; }  // Дата публикации
}
```

### База данных

База данных `news.db` содержит таблицу `SeenNews`:

```sql
CREATE TABLE SeenNews (
    Hash TEXT PRIMARY KEY
);
```

Каждая новость хешируется через SHA256 для предотвращения дубликатов.

## 🛡 Безопасность

⚠️ **Важно!** Не размещайте в публичном репозитории:
- `appsettings.json` с реальными токенами (текущий файл размещен в качестве примера)
- Используйте `.gitignore` для защиты чувствительных данных

Пример `.gitignore`:
```
*.db
appsettings.json
bin/
obj/
```

## 📊 Логирование

Бот использует встроенную систему логирования .NET:
- `Information` — начало проверки, найденные новости
- `Error` — ошибки парсинга и обработки

Логи выводятся в консоль при запуске.

## 🔗 Ссылки

- [Telegram Bot API](https://core.telegram.org/bots/api)
- [.NET Documentation](https://docs.microsoft.com/dotnet/)
- [RSS Specification](https://www.rssboard.org/rss-specification)

## 📄 Лицензия

Проект распространяется под лицензией [MIT](LICENSE).

## 👥 Contributing

Вклад в проект приветствуется! Откройте issue для обсуждения новых фич или багов.

---

Создано с ❤️ для любителей технологий