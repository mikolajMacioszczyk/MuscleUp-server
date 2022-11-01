package groups.common.innerCommunicators.errors;

public class NoAuthHeaderException extends RuntimeException {

    public NoAuthHeaderException(String message) {

        super(message);
    }
}
