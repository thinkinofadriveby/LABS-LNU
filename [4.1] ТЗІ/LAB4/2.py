class LFSR: # Linear Feedback Shift Register
    def __init__(self, init_state, polynomial):
        self.init_state = list(init_state)
        self.state = list(init_state)
        self.polynomial = polynomial

    def step(self):
        feedback = 0
        for i in range(len(self.state)):
            feedback ^= int(self.polynomial[i]) & self.state[i]
        self.state.append(feedback)
        return self.state.pop(0)

    def reset(self):
        self.state = list(self.init_state)

    def encrypt_decrypt(self, message):
        return ''.join(chr(ord(c) ^ self.step()) for c in message)

# Ініціалізуємо стан регістра
init_state = [1, 0, 1, 0, 1, 1, 0, 1, 0, 1, 1]

# Визначаємо многочлен для скремблера
polynomial = [0]*11
polynomial[10] = polynomial[1] = polynomial[0] = 1

# Створюємо об'єкт LFSR
lfsr = LFSR(init_state, polynomial)

# Тестове повідомлення
message = "Hello, world!"

# Шифруємо повідомлення
encrypted_message = lfsr.encrypt_decrypt(message)
print(f"Зашифроване повідомлення: {encrypted_message}")

# Скидаємо стан регістра до початкового стану
lfsr.reset()

# Дешифруємо повідомлення
decrypted_message = lfsr.encrypt_decrypt(encrypted_message)
print(f"Розшифроване повідомлення: {decrypted_message}")