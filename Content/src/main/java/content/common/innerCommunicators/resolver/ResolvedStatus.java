package content.common.innerCommunicators.resolver;

import org.springframework.http.HttpStatus;

public record ResolvedStatus(HttpStatus httpStatus, String description) {
}
