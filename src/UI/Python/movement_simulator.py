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
    factor = np.sqrt((1 + e) / (1 - e))
    tan_v_over_2 = factor * np.tan(E / 2.0)
    v = 2.0 * np.arctan(tan_v_over_2)

    
    v = np.mod(v, 2.0 * np.pi) # normalize to [0, 2π)
    return v


def orbital_radius(a, e, E):
    return a * (1.0 - e * np.cos(E))

def rotation_matrix_3d(argument_of_periapsis, inc, ascending_node):
    """
    Macierz rotacji z płaszczyzny orbity do układu inercjalnego.
    Kolejność: Rz(ascending_node) * Rx(inc) * Rz(argument_of_periapsis)
    """
    cO = np.cos(ascending_node)
    sO = np.sin(ascending_node)
    ci = np.cos(inc)
    si = np.sin(inc)
    co = np.cos(argument_of_periapsis)
    so = np.sin(argument_of_periapsis)

    # Rz(ascending_node)
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

    # Rz(argument_of_periapsis)
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
        self.ascending_node = orbit.ascendingNode
        self.argument_of_periapsis = orbit.argumentOfPeriapsis
        self.M0 = orbit.meanAnomaly
        self.mu = orbit.gravitationalParameter
        self.epoch = 0.0

        # Średni ruch
        self.n = np.sqrt(self.mu / self.a**3)

        # Macierz rotacji z płaszczyzny orbity do inercjalnego
        self.R = rotation_matrix_3d(self.argument_of_periapsis, self.i, self.ascending_node)

    

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
    ascending_node = np.deg2rad(30)
    argument_of_periapsis = np.deg2rad(40)
    M0 = np.deg2rad(0)  # w peryapsie
    epoch = 0.0

    orbit = MovementSimulator(a, e, i, ascending_node, argument_of_periapsis, M0, mu_earth, epoch)

    # Pozycja po 10 minutach
    t = 600.0
    r_vec = orbit.state_vector(t)
    print("r(t) =", r_vec, "m")