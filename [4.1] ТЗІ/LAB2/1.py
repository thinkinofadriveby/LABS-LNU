def create_square(key):
    alphabet = 'ABCDEFGHIJKLMNOPQRSTUVWXYZ'
    key = key.upper()
    key = ''.join(sorted(set(key), key=key.index)) # приведення ключ до унікальних літер, видаляючи дублікати
    # далі відбувається сортування цих літер у порядку в якому вони зустрічаються в оригінальному ключі
    square = key + ''.join(sorted(set(alphabet) - set(key))) # побудова самого квадрата Уітсона
    # перший рядок цього квадрата - це літери ключа, інші літери яких нема в ключі додаються в інші рядки
    # всі літери в квадраті розміщені у такий спосіб, що жодна літера не повторюється,
    # і вони розташовані в алфавітному порядку відповідно до ключа
    return square # повертаємо квадрат

def encrypt(text, square):
    text = text.upper().replace(' ', '')
    encrypted_text = ''
    for char in text:
        if char in square: # перевірка, чи символ char знаходиться у квадраті Уітсона
            index = square.index(char)
            encrypted_text += square[(index + 5) % 25]
    return encrypted_text

def decrypt(encrypted_text, square):
    decrypted_text = ''
    for char in encrypted_text:
        if char in square:
            index = square.index(char)
            decrypted_text += square[(index - 5) % 25]
    return decrypted_text

def main():
    key = input("Input key (letters without spaces): ")
    square = create_square(key)

    choice = input("Select the option  (encrypt - 'e', decrypt - 'd'): ")

    if choice == 'e':
        input_file = input("Input filename with text you want to encrypt: ")
        with open(input_file, 'r') as file:
            text = file.read()
        encrypted_text = encrypt(text, square)
        output_file = input("Input destination filename: ")
        with open(output_file, 'w') as file:
            file.write(encrypted_text)
        print(f"\nEncrypted and saved successfully into {output_file}!")
    elif choice == 'd':
        input_file = input("Input filename with text you want to decrypt: ")
        with open(input_file, 'r') as file:
            encrypted_text = file.read()
        decrypted_text = decrypt(encrypted_text, square)
        print("Decrypted text: ")
        print(decrypted_text)
    else:
        print("Incorrect option selected. Select 'e' for encryption or 'd' for decryption.")

if __name__ == "__main__":
    main()
