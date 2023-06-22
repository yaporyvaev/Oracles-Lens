import { createEvent, createStore } from "effector";

export const $backButton = createStore<boolean>(false);
export const backButtonSetted = createEvent<boolean>();

$backButton.on(backButtonSetted, (_, data) => data);
