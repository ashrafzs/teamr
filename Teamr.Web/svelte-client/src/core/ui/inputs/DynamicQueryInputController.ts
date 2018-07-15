import * as umf from "core-framework";
import * as axiosLib from "axios";

var axios = axiosLib.default;

export class DynamicQueryInputController extends umf.InputController<DynamicQueryValue> {
	filters: QueryFilter[] = [];

	serializeValue(value: DynamicQueryValue | string): any {
		var parsed = this.parse(value);
		return JSON.stringify(parsed);
	}

	init(value: any): Promise<DynamicQueryInputController> {
		return new Promise((resolve, reject) => {
			this.value = this.parse(value);
			resolve(this);
		});
	}

	getValue(): Promise<DynamicQueryValue> {
		var value = this.parse(new DynamicQueryValue(this.value.inputs, this.value.binaryOperators, 
		this.filters.filter(a => a.inputId != null && a.inputId != "")));
		var promises = [];
		for (let filter of this.filters) {
			let controller = filter.inputField;
			if (controller != null){
				let p = controller.serialize().then(t => {
					filter.operand = t.value;
				});

				promises.push(p);
			}		
		}

		return Promise.all(promises).then(() => {
			return value;
		});
	}

	private parse(value: DynamicQueryValue | string): DynamicQueryValue {
		if (value == null) {
			return new DynamicQueryValue();
		}
		var parsed = typeof(value) === "string" 
			? JSON.parse(value)
			: parsed = value;

		return parsed == null || typeof(parsed.inputs) === "undefined"
			? new DynamicQueryValue()
			: parsed;
	}
}

class DynamicQueryValue {
	constructor(inputs: QueryFilter[] = [], binaryOperators: string[] = [], filters: QueryFilter[] = []) {
		this.inputs = inputs;
		this.binaryOperators = binaryOperators;
		this.filters = filters;
	}

	inputs: QueryFilter[] = [];
	filters: QueryFilter[] = [];
	binaryOperators: string[] = [];
}

class QueryFilter{
	public inputId: string;
	public label: string;
	public type: string;
	public operand: string;
	public operators: string[];
	public inputField: umf.InputController<any> = null;
}