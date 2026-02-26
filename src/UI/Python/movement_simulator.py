import numpy as np

KEPLER_TOL = 1e-10
KEPLER_MAX_IT = 50


def solve_kepler(M, e):
    """
    Solve: M = E - e * sin(E)
    """

    M = np.mod(M, 2.0 * np.pi)
    if M > np.pi:
        M -= 2.0 * np.pi

    if e < 0.8:
        E = M
    else:
        E = np.pi

    for _ in range(KEPLER_MAX_IT):
        f = E - e * np.sin(E) - M
        f_prime = 1.0 - e * np.cos(E)
        dE = -f / f_prime
        E += dE
        if abs(dE) < KEPLER_TOL:
            break

    return E


def eccentric_to_true_anomaly(E, e):
    """
    Konwersja anomalii mimośrodowej E do anomalii prawdziwej ν.
    """
    cos_E = np.cos(E)
    sin_E = np.sin(E)

    factor = np.sqrt((1 + e) / (1 - e))
    tan_v_over_2 = factor * np.tan(E / 2.0)
    v = 2.0 * np.arctan(tan_v_over_2)

    # Normalizacja do [0, 2π)
    v = np.mod(v, 2.0 * np.pi)
    return v


def orbital_radius(a, e, E):
    """
    Promień orbity:
        r = a (1 - e cos E)
    """
    return a * (1.0 - e * np.cos(E))


def rotation_matrix_3d(omega, inc, Omega):
    """
    Macierz rotacji z płaszczyzny orbity do układu inercjalnego.
    Kolejność: Rz(Omega) * Rx(inc) * Rz(omega)
    """
    cO = np.cos(Omega)
    sO = np.sin(Omega)
    ci = np.cos(inc)
    si = np.sin(inc)
    co = np.cos(omega)
    so = np.sin(omega)

    # Rz(Omega)
    Rz_O = np.array([
        [cO, -sO, 0.0],
        [sO,  cO, 0.0],
        [0.0, 0.0, 1.0]
    ])

    # Rx(i)
    Rx_i = np.array([
        [1.0, 0.0, 0.0],
        [0.0,  ci, -si],
        [0.0,  si,  ci]
    ])

    # Rz(omega)
    Rz_o = np.array([
        [co, -so, 0.0],
        [so,  co, 0.0],
        [0.0, 0.0, 1.0]
    ])

    # Łączna rotacja
    return Rz_O @ Rx_i @ Rz_o


class MovementSimulator:
    def __init__(self, orbit):
        """
        semi_major_axis: a (m lub dowolne jednostki)
        eccentricity: e
        inclination: i (radiany)
        ascending_node: Ω (radiany)
        arg_periapsis: ω (radiany)
        mean_anomaly_at_epoch: M0 (radiany, w chwili 'epoch')
        mu: parametr grawitacyjny centralnego ciała (GM)
        epoch: czas odniesienia (np. 0.0)
        """
        self.a = orbit.semiMajorAxis
        self.e = orbit.eccentricity
        self.i = orbit.inclination
        self.Omega = orbit.ascendingNode
        self.omega = orbit.argumentOfPeriapsis
        self.M0 = 0.0
        self.mu = 100
        self.epoch = 0.0

        # Średni ruch
        self.n = np.sqrt(self.mu / self.a**3)

        # Macierz rotacji z płaszczyzny orbity do inercjalnego
        self.R = rotation_matrix_3d(self.omega, self.i, self.Omega)

    def mean_anomaly(self, t):
        """
        Anomalia średnia w chwili t:
            M(t) = M0 + n (t - epoch)
        """
        return self.M0 + self.n * (t - self.epoch)

    def state_vector(self, t):
        """
        Zwraca pozycję (3,) w układzie inercjalnym w chwili t.
        (Można łatwo rozszerzyć o prędkość.)
        """
        M = self.mean_anomaly(t)
        E = solve_kepler(M, self.e)
        v = eccentric_to_true_anomaly(E, self.e)
        r = orbital_radius(self.a, self.e, E)

        # Pozycja w płaszczyźnie orbity
        x_orb = r * np.cos(v)
        y_orb = r * np.sin(v)
        z_orb = 0.0

        r_orb = np.array([x_orb, y_orb, z_orb])

        # Transformacja do układu inercjalnego
        r_inertial = self.R @ r_orb
        return r_inertial


# Example usage
if __name__ == "__main__":
    mu_earth = 3.986004418e14  # m^3/s^2

    a = 7000e3          # 7000 km
    e = 0.01
    i = np.deg2rad(45)  # 45°
    Omega = np.deg2rad(30)
    omega = np.deg2rad(40)
    M0 = np.deg2rad(0)  # w peryapsie
    epoch = 0.0

    orbit = KeplerOrbit(a, e, i, Omega, omega, M0, mu_earth, epoch)

    # Pozycja po 10 minutach
    t = 600.0
    r_vec = orbit.state_vector(t)
    print("r(t) =", r_vec, "m")