import * as umf from "core-framework";

export class DateInputController extends umf.InputController<Date> {
	valueAsText: string = null;

	init(value: string): Promise<DateInputController> {
		return new Promise((resolve, reject) => {
			this.value = this.parseDate(value);
			this.valueAsText = this.serializeValue(this.value);

			resolve(this);
		});
	}

	getValue(): Promise<Date> {
		var date = this.parseDate(this.valueAsText);
		return Promise.resolve(date);
	}

	serializeValue(date: Date | string): string {
		var asDate = typeof(date) === "string" 
			? this.parseDate(date)
			: date;

		return asDate != null
			? `${asDate.getFullYear()}-${this.format2DecimalPlaces(asDate.getMonth() + 1)}-${this.format2DecimalPlaces(asDate.getDate())}`
			: null;
	}
	

	private parseDate(value: string): Date {
		let selectedDate = this.asUtcTime(value, 7, 0, 0);
		if (selectedDate)
		{
			let dateAsNumber = Date.parse(selectedDate.toString());
			return isNaN(dateAsNumber) ? null : new Date(dateAsNumber);
		}
	
	}

	private asUtcTime(date, hour, min, second) {
		/// <summary>Returns provided date as if it was UTC date.</summary>
		/// <param name="date">Local date/time.</param>
		/// <returns type="Date">Date object.</returns>
		if (date == null) {
			return null;
		}

		// If string but not UTC.
		if (typeof (date) === "string" && date[date.length - 1] !== "Z") {
			// Assume UTC.
			return new Date(date + "Z");
		}

		var datepart = new Date(new Date(date).toISOString());

		var iso = datepart.getFullYear() +
			"-" + // year
			this.format2DecimalPlaces(datepart.getMonth() + 1) +
			"-" + // month
			this.format2DecimalPlaces(datepart.getDate()) + // day
			"T" +
			this.format2DecimalPlaces(hour) +
			":" +
			this.format2DecimalPlaces(min) +
			":" +
			this.format2DecimalPlaces(second) +
			".000Z";

		return new Date(iso);
	}

	private format2DecimalPlaces(n) {
		return ("0" + n).slice(-2);
	}
}