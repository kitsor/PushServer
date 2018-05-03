namespace WebPushServiceWorker {

	interface IPushNotification {
		title: string;
		message: string;
		iconUrl: string;
		imageUrl: string;
		url: string;
		actions: any[];

	}

	declare abstract class Notification {
		public static maxActions: number;
	}

	declare abstract class WindowClient {
		public url: string;
		public focus(): Promise<WindowClient>;
	}

	declare abstract class clients {
		public static matchAll(params: {
			type: string,
			includeUncontrolled: boolean
		}): Promise<WindowClient[]>;

		public static openWindow(url: string): Promise<WindowClient>;
	}
	
	self.addEventListener('push', function (event: any) {

		if (event.data) {
			const data = event.data.json();

			const notification = data.notification || {};
			const title = getTitle(notification);
			const options = getOptions(notification);

			var callback = getCallback(data);
			if (callback !== null) {
				const promises = Promise.all([
					callback,
					self.registration.showNotification(title, options)
				]);
				event.waitUntil(promises);
			}	else {
				event.waitUntil(self.registration.showNotification(title, options));
			}
		}	else { }
	});

	self.addEventListener('notificationclick', function (event: any) {
		const clickedNotification = event.notification;
		clickedNotification.close();

		const url = clickedNotification.data && clickedNotification.data.url
			? clickedNotification.data.url
			: '/';
		const urlToOpen = new URL(url, self.location.origin).href;

		const promiseChain = clients.matchAll({
			type: 'window',
			includeUncontrolled: true
		})
			.then(windowClients => {
				var matchingClient = null;

				for (var i = 0; i < windowClients.length; i++) {
					var windowClient = windowClients[i];
					if (windowClient.url === urlToOpen) {
						matchingClient = windowClient;
						break;
					}
				}

				if (matchingClient) {
					return matchingClient.focus();
				} else {
					return clients.openWindow(urlToOpen);
				}
			});

		event.waitUntil(promiseChain);
	});


	function getTitle(data: IPushNotification): string {
		return data.title || "";
	}

	function getOptions(data: IPushNotification) {
		const actions: any[] = data.actions && Array.isArray(data.actions) && data.actions.length > 0
			? data.actions.slice(0, Math.min(Notification.maxActions, data.actions.length))
			: [];

		const options = {
			body: data.message || null,
			icon: data.iconUrl || null,
			image: data.imageUrl || null,
			actions: actions,
			data: {
				url: data.url || null,
			}
		};

		return options;
	}

	function getCallback(data: any): Promise<Response> | null {
		if (!data.callbackUrl)
			return null;

		return fetch(data.callbackUrl, {
			headers: {
				'Accept': 'application/json',
				'Content-Type': 'application/json'
			},
			method: 'GET',
		})
	}

}
