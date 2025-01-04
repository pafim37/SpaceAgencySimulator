import { useState } from "react";
import * as Yup from "yup";

export const useBodyValidate = () => {
  const [validateErrors, setValidateErrors] = useState<string[]>([]);

  const vectorSchema = (fieldName: string) =>
    Yup.object({
      x: Yup.number()
        .required()
        .typeError(`${fieldName} x must be a valid number.`),
      y: Yup.number()
        .required()
        .typeError(`${fieldName} y must be a valid number.`),
      z: Yup.number()
        .required()
        .typeError(`${fieldName} z must be a valid number.`),
    });

  const bodySchema = Yup.object().shape({
    name: Yup.string().required("Name is required."),
    mass: Yup.number()
      .typeError("Mass must be a valid number.")
      .required()
      .positive("Mass must be a positive number."),
    radius: Yup.number()
      .typeError("Mass must be a valid number.")
      .required()
      .positive("Mass must be a positive number."),
    position: vectorSchema("Position").required(),
    velocity: vectorSchema("Velocity").required(),
  });

  const validateBody = (newBody: BodyType) => {
    bodySchema
      .validate(newBody, { abortEarly: false })
      .then(() => {
        setValidateErrors([]);
      })
      .catch((err) => {
        setValidateErrors(err.errors);
      });
  };

  return { validateBody, validateErrors };
};

export default useBodyValidate;
