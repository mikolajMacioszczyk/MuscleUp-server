package content.common.user;

import org.springframework.http.HttpStatus;

import java.util.UUID;

public interface UserQuery {

    HttpStatus checkUserId(UUID userId);

    User getUserById(UUID userId);
}
