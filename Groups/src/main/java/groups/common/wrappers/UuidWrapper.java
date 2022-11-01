package groups.common.wrappers;

import groups.common.annotation.MustExist;
import groups.common.annotation.Reason;

import java.util.UUID;

@MustExist(reason = Reason.HIBERNATE)
public record UuidWrapper(UUID uuid) {
}
