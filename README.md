# Space Agency Simulator
Space Agency Simulator is a project which aim is to understand the fundamentals of astrodynamics by the practice

---

# Mathematica Service

## Vector

3-dimensional vector

## Matrix

n-dimensional square Matrix

---

# Astronomy Service

## Observations 
* **GET**  https://localhost:5001/observations (_NotTested_)
* **GET**  https://localhost:5001/observations/{name} (_NotTested_)
* **GET**  https://localhost:5001/observations/extend/{id} (_NotTested_)
* **POST** https://localhost:5001/observations/create-observation (_NotTested_)

## Observatory

* **GET** https://localhost:5001/observatories (_NotTested_)
* **GET** https://localhost:5001/observatories/{IdOrName} (_NotTested_)
* **GET** https://localhost:5001/observatories/{IdOrName}/create-instant-observation (_NotTested_)
 
---
# Body Service 
* **GET** https://localhost:5001/bodies (_NotTested_)
* **GET** https://localhost:5001/bodies/{name} (_NotTested_)
* **POST** https://localhost:5001/bodies (_NotTested_)
* **DELETE** https://localhost:5001/bodies/{name} (_NotTested_)
* **PUT** https://localhost:5001/bodies/{name} (_NotTested_)


---

# Identity Service