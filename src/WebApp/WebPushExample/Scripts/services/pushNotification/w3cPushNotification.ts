import { PushNotificationBase } from './pushNotificationBase'
import { ISubscriptionInfo, W3cSubscriptionInfo } from './subscriptionInfo'
import { StringUtils } from '../../utils/string-utils'

declare class Notification {
	public static permission: string;
}

class W3cPushNotification extends PushNotificationBase {

	public async init(): Promise<this> {
		await navigator.serviceWorker.register(this._params.serviceWorkerUrl);
		return Promise.resolve(this);
	}

	public isSupported(): boolean {
		return 'serviceWorker' in navigator
			&& 'PushManager' in window
			&& 'Notification' in window;
	}

	public hasPermission(): boolean {
		if (!this.isSupported()) return false;
		return this._getPermission() === 'granted';
	}

	public isDisabled(): boolean {
		if (!this.isSupported()) return true;
		return this._getPermission() === 'denied';
	}

	public async subscribe(): Promise<ISubscriptionInfo> {
		const swr = await navigator.serviceWorker.getRegistration();

		const subscription = await swr.pushManager.subscribe(
			{
				userVisibleOnly: true,
				applicationServerKey: StringUtils.urlB64ToUint8Array(this._params.appPublicKey)
			});

		return new W3cSubscriptionInfo(subscription);
	}

	protected _getPermission(): string {
		return Notification.permission;
	}
}

export { W3cPushNotification }
