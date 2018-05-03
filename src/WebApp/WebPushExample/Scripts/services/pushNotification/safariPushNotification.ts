import { IPushNotificationParams, PushNotificationBase } from './pushNotificationBase'
import { ISubscriptionInfo, SafariSubscriptionInfo } from './subscriptionInfo'

declare class Notification {
	public static permission: string;
}

interface IPermissionResponse {
	deviceToken: string;
}

class SafariPushNotification extends PushNotificationBase {

	public init(): Promise<this> {
		return Promise.resolve(this);
	}

	public isSupported(): boolean {
		return 'safari' in window 
			&& 'pushNotification' in window.safari
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
		const subscription = await this._subscribeInternal();
		if (subscription.deviceToken == null) {
			throw new Error("deviceToken is null. PermissionResponse: " + JSON.stringify(subscription));
		}
		return new SafariSubscriptionInfo(subscription.deviceToken);
	}

	protected _subscribeInternal(): Promise<IPermissionResponse> {
		const promise = new Promise<IPermissionResponse>(resolve => {
			window.safari.pushNotification.requestPermission(
				this._params.webServiceUrl,
				this._params.websitePushId,
				{ },
				resolve
			);
		});

		return promise;
	}

	protected _getPermission(): string {
		return Notification.permission;
	}
}

export { SafariPushNotification }