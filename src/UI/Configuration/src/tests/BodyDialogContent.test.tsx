import React from "react";
import { render, screen } from "@testing-library/react";
import BodyDialogContent from "../dialogs/BodyDialogContent";
import "@testing-library/jest-dom";

const bodyMock: BodyType = {
  name: "Planet",
  mass: 6,
  radius: 7,
  enabled: true,
  position: { x: 1, y: 2, z: 3 },
  velocity: { x: 4, y: 5, z: 6 },
};

describe("BodyDialogContent", () => {
  const bodyDialogContent = (isNameDisabled: boolean) => {
    return (
      <BodyDialogContent
        body={bodyMock}
        setBody={jest.fn()}
        isNameDisabled={isNameDisabled}
      />
    );
  };

  test("BodyDialogContent renders properly", () => {
    render(bodyDialogContent(false));
    expect(screen.getByRole("textbox", { name: /name/i })).toHaveDisplayValue(
      "Planet"
    );
    expect(screen.getByRole("textbox", { name: /mass/i })).toHaveDisplayValue(
      "6"
    );
    expect(screen.getByRole("textbox", { name: /radius/i })).toHaveDisplayValue(
      "7"
    );
    expect(
      screen.getByRole("textbox", { name: /X Position/i })
    ).toHaveDisplayValue("1");
    expect(
      screen.getByRole("textbox", { name: /Y Position/i })
    ).toHaveDisplayValue("2");
    expect(
      screen.getByRole("textbox", { name: /Z Position/i })
    ).toHaveDisplayValue("3");
    expect(
      screen.getByRole("textbox", { name: /X Velocity/i })
    ).toHaveDisplayValue("4");
    expect(
      screen.getByRole("textbox", { name: /Y Velocity/i })
    ).toHaveDisplayValue("5");
    expect(
      screen.getByRole("textbox", { name: /Z Velocity/i })
    ).toHaveDisplayValue("6");
  });
});
