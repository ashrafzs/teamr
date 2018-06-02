import {
	InputFieldEventHandler,
	InputController,
	FormInstance,
	InputEventArguments
} from "../../framework/index";
import * as umf from "uimf-core";

export class DependOn extends InputFieldEventHandler {
	run(input: InputController<any>, eventHandlerMetadata: umf.EventHandlerMetadata, args: InputEventArguments): Promise<any> {
		return input.serialize().then(t => {
			var childInput = args.form.getInputComponent(input.metadata.id);
			var parentInput = args.form.getInputComponent(eventHandlerMetadata.customProperties.field); // multiselect.html
			var inputWrapper = childInput.get("wrapper");
			var parentInputController = parentInput.get("field");
			// Special case for multiselect.html.
			var parentInputValue = parentInputController.metadata.type == "typeahead"
				? parentInputController.value.value
				: parentInputController.value;
			var visible = parentInputValue != null;
			inputWrapper.show(visible);
		});
	}
}