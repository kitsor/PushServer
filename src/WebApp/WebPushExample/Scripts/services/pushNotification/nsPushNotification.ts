import { PushNotificationBase } from './pushNotificationBase'
import { ISubscriptionInfo, W3cSubscriptionInfo } from './subscriptionInfo'
import { StringUtils } from '../../utils/string-utils'

class NsPushNotification extends PushNotificationBase {

	public async init(): Promise<this> {
		return Promise.resolve(this);
	}

	public isSupported(): boolean {
		return false;
	}

	public hasPermission(): boolean {
		return false;
	}

	public isDisabled(): boolean {
		return true;
	}

	public async subscribe(): Promise<ISubscriptionInfo> {
		throw new Error('Not supported');
	}
}

export { NsPushNotification }
