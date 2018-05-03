import { ISubscriptionInfo } from './pushNotification/subscriptionInfo'

class ApiClient {

	constructor(
		private _subscriptionsEndpoint: string
	) {
	} 

	public async save(subscriptionInfo: ISubscriptionInfo): Promise<string> {
		const data = subscriptionInfo.toJSON();

		const response = await fetch(this._subscriptionsEndpoint, {
			headers: {
				'Accept': 'application/json',
				'Content-Type': 'application/json'
			},
			method: "POST",
			body: data
		});

		var contentType = response.headers.get("content-type");
		if (response.ok && contentType && contentType.includes("application/json")) {
			const json = await response.json();
			if ('errorCode' in json && json.errorCode == 0) {
				return json.data.userId;
			}
		}

		return null;
	}

	public async delete(userId: string): Promise<boolean> {

		const response = await fetch(this._subscriptionsEndpoint + `/uid/${userId}`, {
			headers: {
				'Accept': 'application/json',
				'Content-Type': 'application/json'
			},
			method: "DELETE"
		});

		var contentType = response.headers.get("content-type");
		if (response.ok) {
			return true;
		}

		return false;
	}
}

export { ApiClient }