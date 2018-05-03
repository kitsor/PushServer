import { ISubscriptionInfo } from './subscriptionInfo'

interface IPushNotification {
	init(): Promise<this>;
	isSupported(): boolean;
	hasPermission(): boolean;
	isDisabled(): boolean;
	subscribe(): Promise<ISubscriptionInfo>;
}


interface IPushNotificationParams {
	serviceWorkerUrl: string;
	appPublicKey: string;
	webServiceUrl: string;
	websitePushId: string;
}

abstract class PushNotificationBase {

	constructor(protected _params: IPushNotificationParams) {
	}
}

export { PushNotificationBase, IPushNotification, IPushNotificationParams }