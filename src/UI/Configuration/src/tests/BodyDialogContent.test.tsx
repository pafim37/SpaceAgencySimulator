import React from "react";
import { fireEvent, render, screen, waitFor } from "@testing-library/react";
import BodyDialogContent from "../dialogs/BodyDialogContent";
import "@testing-library/jest-dom";

const bodyMock: BodyStringType = {
  name: "Planet",
  mass: "42",
  radius: "37",
  position: { x: "1", y: "2", z: "3" },
  velocity: { x: "4", y: "5", z: "6" },
};

describe("BodyDialogContent", () => {
  const bodyDialogContent = () => {
    return (
      <BodyDialogContent
        body={bodyMock}
        setBody={jest.fn()}
        setIsValidBodyForm={jest.fn()}
      />
    );
  };

  test("BodyDialogContent renders properly", () => {
    render(bodyDialogContent());
    expect(screen.getByRole("textbox", { name: /name/i })).toHaveDisplayValue(
      "Planet"
    );
    expect(screen.getByRole("textbox", { name: /mass/i })).toHaveDisplayValue(
      "42"
    );
    expect(screen.getByRole("textbox", { name: /radius/i })).toHaveDisplayValue(
      "37"
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

  test("BodyDialogContent mass, radius, position and velocity don't get string", async () => {
    render(bodyDialogContent());
    const text = "abc";
    const mass = screen.getByRole("textbox", { name: /mass/i });
    await waitFor(() => fireEvent.change(mass, text));
    expect(mass).toHaveDisplayValue("42");

    const radius = screen.getByRole("textbox", { name: /radius/i });
    await waitFor(() => fireEvent.change(radius, text));
    expect(radius).toHaveDisplayValue("37");

    const positionX = screen.getByRole("textbox", { name: /X Position/i });
    await waitFor(() => fireEvent.change(positionX, text));
    expect(positionX).toHaveDisplayValue("1");

    const positionY = screen.getByRole("textbox", { name: /Y Position/i });
    await waitFor(() => fireEvent.change(positionY, text));
    expect(positionY).toHaveDisplayValue("2");

    const positionZ = screen.getByRole("textbox", { name: /Z Position/i });
    await waitFor(() => fireEvent.change(positionZ, text));
    expect(positionZ).toHaveDisplayValue("3");

    const velocityX = screen.getByRole("textbox", { name: /X Velocity/i });
    await waitFor(() => fireEvent.change(velocityX, text));
    expect(velocityX).toHaveDisplayValue("4");

    const velocityY = screen.getByRole("textbox", { name: /Y Velocity/i });
    await waitFor(() => fireEvent.change(velocityY, text));
    expect(velocityY).toHaveDisplayValue("5");

    const velocityZ = screen.getByRole("textbox", { name: /Z Velocity/i });
    await waitFor(() => fireEvent.change(velocityZ, text));
    expect(velocityZ).toHaveDisplayValue("6");
  });

  test("BodyDialogContent mass, radius, position and velocity get number", async () => {
    render(bodyDialogContent());
    const text = "13";
    const result = "13";

    const mass = screen.getByRole("textbox", { name: /mass/i });
    await waitFor(() => fireEvent.change(mass, { target: { value: text } }));
    expect(mass).toHaveDisplayValue(result);

    const radius = screen.getByRole("textbox", { name: /radius/i });
    await waitFor(() => fireEvent.change(radius, { target: { value: text } }));
    expect(radius).toHaveDisplayValue(result);

    const positionX = screen.getByRole("textbox", { name: /X Position/i });
    await waitFor(() =>
      fireEvent.change(positionX, { target: { value: text } })
    );
    expect(positionX).toHaveDisplayValue(result);

    const positionY = screen.getByRole("textbox", { name: /Y Position/i });
    await waitFor(() =>
      fireEvent.change(positionY, { target: { value: text } })
    );
    expect(positionY).toHaveDisplayValue(result);

    const positionZ = screen.getByRole("textbox", { name: /Z Position/i });
    await waitFor(() =>
      fireEvent.change(positionZ, { target: { value: text } })
    );
    expect(positionZ).toHaveDisplayValue(result);

    const velocityX = screen.getByRole("textbox", { name: /X Velocity/i });
    await waitFor(() =>
      fireEvent.change(velocityX, { target: { value: text } })
    );
    expect(velocityX).toHaveDisplayValue(result);

    const velocityY = screen.getByRole("textbox", { name: /Y Velocity/i });
    await waitFor(() =>
      fireEvent.change(velocityY, { target: { value: text } })
    );
    expect(velocityY).toHaveDisplayValue(result);

    const velocityZ = screen.getByRole("textbox", { name: /Z Velocity/i });
    await waitFor(() =>
      fireEvent.change(velocityZ, { target: { value: text } })
    );
    expect(velocityZ).toHaveDisplayValue(result);
  });

  test("BodyDialogContent mass, radius, position and velocity get empty", async () => {
    render(bodyDialogContent());
    const text = "";
    const result = "";

    const mass = screen.getByRole("textbox", { name: /mass/i });
    await waitFor(() => fireEvent.change(mass, { target: { value: text } }));
    expect(mass).toHaveDisplayValue(result);

    const radius = screen.getByRole("textbox", { name: /radius/i });
    await waitFor(() => fireEvent.change(radius, { target: { value: text } }));
    expect(radius).toHaveDisplayValue(result);

    const positionX = screen.getByRole("textbox", { name: /X Position/i });
    await waitFor(() =>
      fireEvent.change(positionX, { target: { value: text } })
    );
    expect(positionX).toHaveDisplayValue(result);

    const positionY = screen.getByRole("textbox", { name: /Y Position/i });
    await waitFor(() =>
      fireEvent.change(positionY, { target: { value: text } })
    );
    expect(positionY).toHaveDisplayValue(result);

    const positionZ = screen.getByRole("textbox", { name: /Z Position/i });
    await waitFor(() =>
      fireEvent.change(positionZ, { target: { value: text } })
    );
    expect(positionZ).toHaveDisplayValue(result);

    const velocityX = screen.getByRole("textbox", { name: /X Velocity/i });
    await waitFor(() =>
      fireEvent.change(velocityX, { target: { value: text } })
    );
    expect(velocityX).toHaveDisplayValue(result);

    const velocityY = screen.getByRole("textbox", { name: /Y Velocity/i });
    await waitFor(() =>
      fireEvent.change(velocityY, { target: { value: text } })
    );
    expect(velocityY).toHaveDisplayValue(result);

    const velocityZ = screen.getByRole("textbox", { name: /Z Velocity/i });
    await waitFor(() =>
      fireEvent.change(velocityZ, { target: { value: text } })
    );
    expect(velocityZ).toHaveDisplayValue(result);
  });

  test("BodyDialogContent mass, radius, position and velocity get float", async () => {
    render(bodyDialogContent());
    const text = "1.2";
    const result = "1.2";

    const mass = screen.getByRole("textbox", { name: /mass/i });
    await waitFor(() => fireEvent.change(mass, { target: { value: text } }));
    expect(mass).toHaveDisplayValue(result);

    const radius = screen.getByRole("textbox", { name: /radius/i });
    await waitFor(() => fireEvent.change(radius, { target: { value: text } }));
    expect(radius).toHaveDisplayValue(result);

    const positionX = screen.getByRole("textbox", { name: /X Position/i });
    await waitFor(() =>
      fireEvent.change(positionX, { target: { value: text } })
    );
    expect(positionX).toHaveDisplayValue(result);

    const positionY = screen.getByRole("textbox", { name: /Y Position/i });
    await waitFor(() =>
      fireEvent.change(positionY, { target: { value: text } })
    );
    expect(positionY).toHaveDisplayValue(result);

    const positionZ = screen.getByRole("textbox", { name: /Z Position/i });
    await waitFor(() =>
      fireEvent.change(positionZ, { target: { value: text } })
    );
    expect(positionZ).toHaveDisplayValue(result);

    const velocityX = screen.getByRole("textbox", { name: /X Velocity/i });
    await waitFor(() =>
      fireEvent.change(velocityX, { target: { value: text } })
    );
    expect(velocityX).toHaveDisplayValue(result);

    const velocityY = screen.getByRole("textbox", { name: /Y Velocity/i });
    await waitFor(() =>
      fireEvent.change(velocityY, { target: { value: text } })
    );
    expect(velocityY).toHaveDisplayValue(result);

    const velocityZ = screen.getByRole("textbox", { name: /Z Velocity/i });
    await waitFor(() =>
      fireEvent.change(velocityZ, { target: { value: text } })
    );
    expect(velocityZ).toHaveDisplayValue(result);
  });

  test("BodyDialogContent mass, radius, position and velocity get scientific notation", async () => {
    render(bodyDialogContent());
    const text = "1e2";
    const result = "1e2";

    const mass = screen.getByRole("textbox", { name: /mass/i });
    await waitFor(() => fireEvent.change(mass, { target: { value: text } }));
    expect(mass).toHaveDisplayValue(result);

    const radius = screen.getByRole("textbox", { name: /radius/i });
    await waitFor(() => fireEvent.change(radius, { target: { value: text } }));
    expect(radius).toHaveDisplayValue(result);

    const positionX = screen.getByRole("textbox", { name: /X Position/i });
    await waitFor(() =>
      fireEvent.change(positionX, { target: { value: text } })
    );
    expect(positionX).toHaveDisplayValue(result);

    const positionY = screen.getByRole("textbox", { name: /Y Position/i });
    await waitFor(() =>
      fireEvent.change(positionY, { target: { value: text } })
    );
    expect(positionY).toHaveDisplayValue(result);

    const positionZ = screen.getByRole("textbox", { name: /Z Position/i });
    await waitFor(() =>
      fireEvent.change(positionZ, { target: { value: text } })
    );
    expect(positionZ).toHaveDisplayValue(result);

    const velocityX = screen.getByRole("textbox", { name: /X Velocity/i });
    await waitFor(() =>
      fireEvent.change(velocityX, { target: { value: text } })
    );
    expect(velocityX).toHaveDisplayValue(result);

    const velocityY = screen.getByRole("textbox", { name: /Y Velocity/i });
    await waitFor(() =>
      fireEvent.change(velocityY, { target: { value: text } })
    );
    expect(velocityY).toHaveDisplayValue(result);

    const velocityZ = screen.getByRole("textbox", { name: /Z Velocity/i });
    await waitFor(() =>
      fireEvent.change(velocityZ, { target: { value: text } })
    );
    expect(velocityZ).toHaveDisplayValue(result);
  });
});
