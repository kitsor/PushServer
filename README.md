# Web Push Notifications Server on .NET Core

Development is still in progress.

- **PushServer.API** (scalable). Subscriptions and notifications manager.
- **PushServer.API.BackgroundTasks** (scalable). Responsible for notifications delivery.
- **PushServer.PNS.ApplePush**. Push client for APN and PushPackage generator.
- **PushServer.PNS.WebPush**. W3C Push client for Google Chrome and Firefox.
- **WebPushExample**. Sample website with client-site subscription manager (subscribe/unsubscribe).

## Todo
- More tests
- Admin UI + IdentityServer
- Segmentation of the subscriptions

# Credits
- WebPush client. Original repo: https://github.com/web-push-libs/web-push-csharp
- Icons - https://icons8.com
- Architecture: https://docs.microsoft.com/en-us/dotnet/standard/microservices-architecture/