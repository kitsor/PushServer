import { Browser, BrowserType } from '../utils/browser'
import { IPushNotification, IPushNotificationParams } from './pushNotification/pushNotificationBase'
import { W3cPushNotification } from './pushNotification/w3cPushNotification'
import { SafariPushNotification } from './pushNotification/safariPushNotification'
import { NsPushNotification } from './pushNotification/nsPushNotification'

class PushNotificationFactory {

	public static create(params: IPushNotificationParams): IPushNotification {
		let instance: IPushNotification = null;

		switch (Browser.getBrowserType()) {
			case BrowserType.Chrome:
			case BrowserType.Firefox:
				instance = new W3cPushNotification(params);
				break;

			case BrowserType.Safari:
				instance = new SafariPushNotification(params);
				break;

			default:
				instance = new NsPushNotification(params);
		}

		return instance;
	}
}

export { PushNotificationFactory }