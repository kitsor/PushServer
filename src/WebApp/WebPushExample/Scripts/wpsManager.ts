import { Browser } from './utils/browser'
import { LocalStorage } from './services/localStorage'
import { IPushNotification, IPushNotificationParams } from './services/pushNotification/pushNotificationBase'
import { PushNotificationFactory } from './services/pushNotificationFactory'
import { ApiClient } from './services/apiClient'

interface IWpsStorage {
	setItem(key:string, item: any): void;
	getItem(key: string): string;
	removeItem(key: string): void;
	hasKey(key: string): boolean;
}

interface IWpsManagerOptions {
	subscriptionsEndpoint: string;
	pushNotificationParams: IPushNotificationParams;
}

class WpsManager {

	private _skey_userId = 'uid';

	private _storage: IWpsStorage;
	private _pushNotification: IPushNotification;
	
	constructor(private _options: IWpsManagerOptions) {
		this._storage = new LocalStorage();
		this._pushNotification = PushNotificationFactory.create(_options.pushNotificationParams);
	}

	public async init(): Promise<WpsManager> {
		if (this._pushNotification === null)
			return Promise.resolve(this);

		await this._pushNotification.init();
		return Promise.resolve(this);
	}

	public isSupported(): boolean {
		return this._pushNotification.isSupported();
	}

	public isSubscribed(): boolean {
		return this._storage.hasKey(this._skey_userId);
	}

	public hasPermission(): boolean {
		return this._pushNotification.hasPermission();
	}

	public isDisabled(): boolean {
		return this._pushNotification.isDisabled();
	}

	public async subscribe(): Promise<boolean> {
		const subscriptionInfo = await this._pushNotification.subscribe();

		try {

			const apiClient = new ApiClient(this._options.subscriptionsEndpoint);
			const userId = await apiClient.save(subscriptionInfo);

			if (userId !== null) {
				this._storage.setItem(this._skey_userId, userId);
				return true;
			}

			return false;

		} catch {
			throw new Error('NetworkError when attempting to connect to subscriptionsEndpoint.')
		}
	}

	public async unsubscribe(): Promise<boolean> {
		if (!this._storage.hasKey(this._skey_userId))
			return;

		const userId = this._storage.getItem(this._skey_userId);
		const apiClient = new ApiClient(this._options.subscriptionsEndpoint);
		const isSuccess = await apiClient.delete(userId);

		if (isSuccess) {
			this._storage.removeItem(this._skey_userId);
			return true;
		}

		return false;
	}
}

export { WpsManager, IWpsManagerOptions };