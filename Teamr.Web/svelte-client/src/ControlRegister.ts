import * as umf from "core-framework";

import { DateInputController } from "core-ui/inputs/DateInputController";
import { NumberInputController } from "core-ui/inputs/NumberInputController";
import { DropdownInputController } from "core-ui/inputs/DropdownInputController";
import { BooleanInputController } from "core-ui/inputs/BooleanInputController";
import { PaginatorInputController } from "core-ui/inputs/PaginatorInputController";
import { MultiSelectInputController } from "core-ui/inputs/MultiSelectInputController";
import { TypeaheadInputController } from "core-ui/inputs/TypeaheadInputController";
import { PasswordInputController } from "core-ui/inputs/PasswordInputController";
import { TextareaInputController } from "core-ui/inputs/TextareaInputController";
import { FileUploaderController } from "core-ui/inputs/FileUploaderController";
import { DynamicFormInputController } from "core-ui/inputs/DynamicFormInputController";

import TextInput from "core-ui/inputs/Text";
import NumberInput from "core-ui/inputs/Number";
import DateInput from "core-ui/inputs/Date";
import DropdownInput from "core-ui/inputs/Dropdown";
import BooleanInput from "core-ui/inputs/Boolean";
import MultiSelectInput from "core-ui/inputs/MultiSelect";
import Password from "core-ui/inputs/Password";
import Textarea from "core-ui/inputs/Textarea";
import FileUploader from "core-ui/inputs/FileUploader";
import DynamicForm from "core-ui/inputs/DynamicForm";

import TextOutput from "core-ui/outputs/Text";
import NumberOutput from "core-ui/outputs/Number";
import DateTimeOutput from "core-ui/outputs/Datetime";
import TableOutput from "core-ui/outputs/Table";
import FormLink from "core-ui/outputs/FormLink";
import Tabstrip from "core-ui/outputs/Tabstrip";
import Paginator from "core-ui/outputs/Paginator";
import ActionList from "core-ui/outputs/ActionList";
import InlineForm from "core-ui/outputs/InlineForm";
import TextValue from "core-ui/outputs/TextValue";
import TextValueMultiline from "core-ui/outputs/TextValueMultiline";
import DownloadableFile from "core-ui/outputs/DownloadableFile";
import Alert from "core-ui/outputs/Alert";
import FileSize from "core-ui/outputs/FileSize";
import Image from "core-ui/outputs/Image";
import Link from "core-ui/outputs/Link";
import DynamicOutput from "core-ui/outputs/DynamicOutput";
import FormInstance from "core-ui/outputs/FormInstance";
import PieGraph from "core-ui/outputs/PieGraph";

import {
	FormLogToConsole,
	BindToOutput,
	InputLogToConsole,
	OutputLogToConsole,
	ReloadFormAfterAction,
	DependOn

} from "core-eventHandlers";

import { Growl } from "core-functions";

var controlRegister = new umf.ControlRegister();
controlRegister.registerInputFieldControl("text", TextInput, umf.StringInputController);
controlRegister.registerInputFieldControl("datetime", DateInput, DateInputController);
controlRegister.registerInputFieldControl("number", NumberInput, NumberInputController);
controlRegister.registerInputFieldControl("dropdown", DropdownInput, DropdownInputController);
controlRegister.registerInputFieldControl("boolean", BooleanInput, BooleanInputController);
controlRegister.registerInputFieldControl("paginator", null, PaginatorInputController);
controlRegister.registerInputFieldControl("typeahead", MultiSelectInput, TypeaheadInputController);
controlRegister.registerInputFieldControl("my-typeahead", MultiSelectInput, TypeaheadInputController);
controlRegister.registerInputFieldControl("multiselect", MultiSelectInput, MultiSelectInputController);
controlRegister.registerInputFieldControl("password", Password, PasswordInputController);
controlRegister.registerInputFieldControl("textarea", Textarea, TextareaInputController, { block: true });
controlRegister.registerInputFieldControl("file-uploader", FileUploader, FileUploaderController, { alwaysHideLabel: true, block: true });
controlRegister.registerInputFieldControl("dynamic-form", DynamicForm, DynamicFormInputController);

controlRegister.registerOutputFieldControl("text", TextOutput);
controlRegister.registerOutputFieldControl("number", NumberOutput);
controlRegister.registerOutputFieldControl("datetime", DateTimeOutput);
controlRegister.registerOutputFieldControl("table", TableOutput, { block: true });
controlRegister.registerOutputFieldControl("formlink", FormLink);
controlRegister.registerOutputFieldControl("tabstrip", Tabstrip, { alwaysHideLabel: true, block: true });
controlRegister.registerOutputFieldControl("paginated-data", Paginator, { block: true });
controlRegister.registerOutputFieldControl("action-list", ActionList, { alwaysHideLabel: true, block: true });
controlRegister.registerOutputFieldControl("inline-form", InlineForm, { alwaysHideLabel: true, block: true });
controlRegister.registerOutputFieldControl("text-value", TextValue);
controlRegister.registerOutputFieldControl("text-value-multiline", TextValueMultiline, { block: true });
controlRegister.registerOutputFieldControl("downloadable-file", DownloadableFile);
controlRegister.registerOutputFieldControl("alert", Alert, { alwaysHideLabel: true, block: true });
controlRegister.registerOutputFieldControl("file-size", FileSize);
controlRegister.registerOutputFieldControl("image", Image, { block: true });
controlRegister.registerOutputFieldControl("link", Link);
controlRegister.registerOutputFieldControl("object-list", DynamicOutput, { block: true });
controlRegister.registerOutputFieldControl("form-instance", FormInstance, { alwaysHideLabel: true });
controlRegister.registerOutputFieldControl("pie-graph", PieGraph, { block: true });

// Form event handlers.
controlRegister.registerFormEventHandler("log-to-console", new FormLogToConsole());
controlRegister.registerFormEventHandler("reload-form-after-action", new ReloadFormAfterAction());

// Input event handlers.
controlRegister.registerInputFieldEventHandler("bind-to-output", new BindToOutput());
controlRegister.registerInputFieldEventHandler("log-to-console", new InputLogToConsole());
controlRegister.registerInputFieldEventHandler("depend-on", new DependOn());

// Output event handlers.
controlRegister.registerOutputFieldEventHandler("log-to-console", new OutputLogToConsole());

// Functions.
controlRegister.registerFunction("growl", new Growl());

export default controlRegister;